// Use StateMachine to switch modes as needed
type ReadMode =
| StandardRead
| InGarbage
| IgnoreNext

type GroupState = {
    ReadMode: ReadMode
    CurrentDepth: int
    Score: int
    GarbageCount: int }
    with static member Initial = { ReadMode = StandardRead; CurrentDepth = 0; Score = 0; GarbageCount = 0 }

let processStream (gs:GroupState) (c:char) =
    match gs.ReadMode with        
    | StandardRead -> 
        match c with 
        | '{' -> { gs with ReadMode = StandardRead; CurrentDepth= gs.CurrentDepth + 1 }
        | '<' -> { gs with ReadMode = InGarbage }
        | '}' ->         
            { gs with 
                ReadMode = StandardRead
                CurrentDepth= gs.CurrentDepth - 1 
                Score = gs.Score + gs.CurrentDepth }
        | _ -> gs
    | InGarbage -> 
        match c with
        | '!' -> { gs with ReadMode = IgnoreNext }
        | '>' -> { gs with ReadMode = StandardRead }
        | _ -> { gs with GarbageCount = gs.GarbageCount + 1 }
    | IgnoreNext -> { gs with ReadMode = InGarbage }

let test partfun testNo input expected =
    let output = 
        input
        |> Seq.fold processStream GroupState.Initial
        |> partfun
    if output = expected then printfn "Test %i passed with expected output %i" testNo output
    else printfn "Test %i failed. Expected: %i Actual: %i" testNo expected output

let test1 = test (fun gs -> gs.Score )

test1 1 @"{}" 1
test1 2 @"{{{}}}" 6
test1 3 @"{{},{}}" 5
test1 4 @"{{{},{},{{}}}}" 16
test1 5 @"{<a>,<a>,<a>,<a>}" 1
test1 6 @"{{<ab>},{<ab>},{<ab>},{<ab>}}" 9
test1 7 @"{{<!!>},{<!!>},{<!!>},{<!!>}}" 9
test1 8 @"{{<a!>},{<a!>},{<a!>},{<ab>}}" 3

let test2 = test (fun gs -> gs.GarbageCount)

test2 9 @"<>" 0
test2 10 @"<random characters>" 17
test2 11 @"<<<<>" 3
test2 12 @"<{!>}>" 2
test2 13 @"<!!>" 0
test2 14 @"<!!!>>" 0
test2 15 @"<{o""i!a,<{i<a>" 10

let output =
    System.IO.File.ReadAllText @"c:\work\adventofcode2017\day9\input.txt"
    |> Seq.fold processStream GroupState.Initial

printfn "Part 1: %i" output.Score

printfn "Part 2: %i" output.GarbageCount 