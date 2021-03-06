type Comparison = int -> int -> bool

type Condition = {
    Register: string
    Comparison: Comparison
    Value: int }

type Instruction = {
    Register: string
    Operation: (int -> int)
    Amount: int
    Condition: Condition }

type Bank = Map<string,int>

let compareFromStr s =
    match s with
    | "==" -> fun a v -> a = v
    | "<" -> fun a v -> a < v
    | "<=" -> fun a v -> a <= v
    | ">" -> fun a v -> a > v
    | ">=" -> fun a v -> a >= v
    | "!=" -> fun a v -> not (a = v)
    | _ -> fun a v -> false    

let parseInstruction (str:string) =
    let i = str.Split(' ')
    let amt = int i.[2]
    { Register = i.[0]
      Operation = match i.[1] with | "inc" -> (fun i -> i+amt) | "dec" -> (fun i -> i-amt) | _ -> (fun i -> i)
      Amount = i.[2] |> int
      Condition = { Register = i.[4]; Comparison = compareFromStr i.[5]; Value = int i.[6] } }

let regValue k bnk = match Map.tryFind k bnk with | Some n -> n | None -> 0

let compute (b:Map<string,int>) (v:int) (i:Instruction) =
    let cv = regValue i.Condition.Register b
    match i.Condition.Comparison cv i.Condition.Value with
    | true -> i.Operation v
    | false -> v
    
let computeBank (b:Map<string,int>) (i:Instruction) =
    let v = regValue i.Register b
    let nv = compute b v i
    Map.add i.Register nv b
    
let computeBankWithMax (b:Map<string,int>,max:int) (i:Instruction) =
    let v = regValue i.Register b
    let nv = compute b v i
    Map.add i.Register nv b, System.Math.Max(max,nv)


let testInput = 
    let testStr = @"b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10"
    testStr.Split('\n')
    |> Seq.map parseInstruction
    |> Seq.cache


let part1 input =
    input
    |> Seq.fold computeBank Map.empty<string,int>
    |> Map.toSeq
    |> Seq.maxBy (fun (k,v) -> v)
    |> snd

let part2 input =
    input
    |> Seq.fold computeBankWithMax (Map.empty<string,int>,System.Int32.MinValue)
    |> snd


part1 testInput |> printfn "part 1 test result: %i"
part2 testInput |> printfn "part 2 test result: %i"

let fileInput = 
    System.IO.File.ReadLines @"C:\work\adventofcode2017\day8\input.txt"
    |> Seq.map parseInstruction
    |> Seq.cache

fileInput
|> part1
|> printfn "part 1: %i"

fileInput
|> part2
|> printfn "part 2: %i"

