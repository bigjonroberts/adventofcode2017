open System
type Group = {
    Id: int
    OrigList: Set<int>
    FullList: Set<int>
}

let parseLine (s:String) =
    let parts = s.Split(' ')
    let n = Seq.head parts |> int
    let rest =
        parts |> Seq.skip 2 
        |> Seq.map (fun t -> t.Trim([|','|]))
        |> Seq.map int 
        |> Set.ofSeq
    n, rest

let parseInput (str:seq<String>) =
    str
    |> Seq.map parseLine
    |> Map.ofSeq

//let rec findGroups (g:Map<int,group>) (pos:int) (pos2:int) =
//    
//    let h = g.[pos]
//    if Set.isProperSuperset h.FullList h.OrigList then
//        g
//    else
//        let ha = { g.[pos] with FullList = Set.add pos2 g.[pos].FullList }
//        let g2 = Map.add pos ha g
//        let moreList =
//            h.OrigList
//            |> Set.map (fun i -> findGroups g pos i)
//            |> Set.map (fun g2 -> Map.toSeq g2 )
//        g

let rec buildGroup (g:Map<int,Set<int>>) (currGroup:Set<int>) (traversed:Set<int>) (pos:int)  =
    let setToStr = Set.toSeq >> Seq.map string >> (fun x -> String.Join(",",x))

    let g2 = Map.add pos currGroup g
    let newTraversed = Set.add pos traversed
    if  newTraversed = traversed then // no items added in last iteration
        printfn "%i traversed = %s\n newTraversed = %s" pos (setToStr traversed) (setToStr newTraversed)
        traversed |> Set.add pos
    else
        let nextSet =
            Set.difference g.[pos] traversed
            |> Set.map (buildGroup g2 currGroup newTraversed)
            |> Set.toSeq
            |> Set.unionMany
            |> Set.add pos
            |> Set.union traversed
        nextSet |> setToStr
        |> printfn "%i next set: %s" pos
        traversed |> setToStr
        |> printfn "... traversed: %s"
        nextSet



parseLine "0 <-> 2"
parseLine "2 <-> 0, 3, 4"

let testInput = "0 <-> 2\n1 <-> 1\n2 <-> 0, 3, 4\n3 <-> 2, 4\n4 <-> 2, 3, 6\n5 <-> 6\n6 <-> 4, 5"
let g1 =
    testInput |> (fun (s:String) -> s.Split('\n'))
    |> parseInput

let gr = 
    Set.toSeq g1.[0] 
    |> Seq.head
    |> buildGroup g1 g1.[0] Set.empty<int>

let zeroCount = gr |> Set.count
zeroCount |> printfn "Test count for 0 group: %i"

let input =
    System.IO.File.ReadLines "c:/work/adventofcode/day12/input.txt"
    |> parseInput

input.[0]
|> Set.toSeq
|> Seq.head
|> buildGroup input input.[0] Set.empty<int>
|> Set.count
|> printfn "part 1: %i"

let rec getGroups (all:Map<int,Set<int>>) curGroups notYetGrouped =
    if notYetGrouped = Set.empty<int> then
        curGroups
    else
        let startingPoint = Set.minElement notYetGrouped
        let newGroup =
            all.[startingPoint]
            |> Set.toSeq
            |> Seq.head
            |> buildGroup all all.[startingPoint] Set.empty<int>
        getGroups all (curGroups |> List.append [newGroup]) (Set.difference notYetGrouped newGroup)

let unGrouped = input |> Map.toSeq |> Seq.map fst |> Set.ofSeq 

let groupCount =
    getGroups input [] unGrouped
    |> List.length

printfn "part 2: %i" groupCount