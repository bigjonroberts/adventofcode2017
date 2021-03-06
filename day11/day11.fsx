open System

type Direction =
| North = 360
| Northeast = 60
| Southeast = 120
| South = 180
| Southwest = 240
| Northwest = 300

type Axis =
| NS
| NESW
| SENW

//type Position = {
//    a1:int
//    a2:int
//    a3:int
//} with
//    member x.n = max x.a1 0
//    member x.s = min x.a1 0 |> abs
//    member x.ne = max x.a2 0
//    member x.sw = min x.a2 0 |> abs
//    member x.se = max x.a3 0
//    member x.nw = min x.a3 0 |> abs

//type Position = {
//    Pos:Map<Direction,int> } with
//    member x.a1 = x.Pos.[Direction.North] - x.Pos.[Direction.South]
//    member x.a2 = x.Pos.[Direction.Northeast] - x.Pos.[Direction.Southwest]
//    member x.a1 = x.Pos.[Direction.Southeast] - x.Pos.[Direction.Northwest]

type Position = {
    n:  int
    ne: int
    se: int
    s:  int
    sw: int
    nw: int } with
    member x.a1 = x.n - x.s
    member x.a2 = x.ne - x.sw
    member x.a3 = x.se - x.nw
    member x.distance = x.n + x.ne + x.se + x.s + x.sw + x.nw

let simplify a b c =
    if a = 0 || c = 0 then
        a,b,c
    else
        if a < c then
            0,b+a,c-a
        else
            a-c,b+c,0

let parseMove s =
    match s with
    | "n" -> Direction.North
    | "ne" -> Direction.Northeast
    | "se" -> Direction.Southeast
    | "s" -> Direction.South
    | "sw" -> Direction.Southwest
    | "nw" -> Direction.Northwest
    | _ -> Direction.North

let processMove p m =
    match m with
    | Direction.North ->     { p with n = p.n + 1 }
    | Direction.Northeast -> { p with ne = p.ne + 1 }
    | Direction.Southeast -> { p with se = p.se + 1 }
    | Direction.South ->     { p with s = p.s + 1 }
    | Direction.Southwest -> { p with sw = p.sw + 1 }
    | Direction.Northwest -> { p with nw = p.nw + 1 }
    | _ -> p

let distill p = // go "round the clock" and consolidate moves
    let n1, ne1,se1 = simplify p.n p.ne p.se
    let ne2,se2,s1  = simplify ne1 se1  p.s
    let se3,s2, sw1 = simplify se2 s1   p.sw
    let s3, sw2,nw1 = simplify s2  sw1  p.nw
    let sw3,nw2,n2  = simplify sw2 nw1  n1
    let nw3,n3 ,ne3 = simplify nw2 n2   ne2
    let n4 = n3-s3 |> max 0
    let s4 = s3-n3 |> max 0
    let ne4 = ne3-sw3 |> max 0
    let sw4 = sw3-ne3 |> max 0
    let se4 = se3-nw3 |> max 0
    let nw4 = nw3-se3 |> max 0
    { n = n4; ne = ne4; se = se4; s = s4; sw = sw4; nw = nw4 }

let processMove2 (p,d:int) m =
    let newP =
        match m with
        | Direction.North ->     { p with n = p.n + 1 }
        | Direction.Northeast -> { p with ne = p.ne + 1 }
        | Direction.Southeast -> { p with se = p.se + 1 }
        | Direction.South ->     { p with s = p.s + 1 }
        | Direction.Southwest -> { p with sw = p.sw + 1 }
        | Direction.Northwest -> { p with nw = p.nw + 1 }
        | _ -> p
        |> distill
        |> distill
        |> distill // have to distill multiple times to get all the way
    newP,newP.distance


let findMoves sm =
    sm
    |> Seq.fold processMove { n=0; ne=0; se=0; s=0; sw=0; nw=0 }
    |> distill
    |> distill // have to distill twice to get all the way

let findMoves2 sm =
    sm
    |> Seq.scan processMove2 ({ n=0; ne=0; se=0; s=0; sw=0; nw=0 },0)
    |> Seq.maxBy snd


"ne,ne,ne"
|> (fun s -> s.Split(','))
|> Seq.map parseMove
|> findMoves

"ne,ne,sw,sw"
|> (fun s -> s.Split(','))
|> Seq.map parseMove
|> findMoves

"ne,ne,s,s"
|> (fun s -> s.Split(','))
|> Seq.map parseMove
|> findMoves

"se,sw,se,sw,sw"
|> (fun s -> s.Split(','))
|> Seq.map parseMove
|> findMoves

let input =
    System.IO.File.ReadAllText "c:/work/adventofcode2017/day11/input.txt"
    |> (fun s -> s.Split(','))
    |> Seq.map parseMove
    |> Seq.cache

input
|> findMoves
|> (fun p -> p.distance )
|> printfn "day 11 part 1: %i"

input
|> findMoves2
|> snd
|> printfn "day 11 part 2: %i"
