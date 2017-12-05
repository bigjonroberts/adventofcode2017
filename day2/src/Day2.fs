namespace AdventOfCode2017

[<AutoOpen>]
module Day2 =
    let span (s:seq<int>) =
        let sc = Seq.cache s
        let min = Seq.min sc
        let max = Seq.max sc
        max - min

    let evenlyDiv (i,j) = 
        match (i,j) with
        | (m,n) when (m=n) -> 0
        | (m,n) when (m % n = 0) -> m/n
        | (m,n) when (n % m = 0) -> n/m
        | _ -> 0

    let div2 (s:seq<int>) =
        let sc = Seq.cache s
        let sr = Seq.rev sc
        sc
        |> Seq.sumBy (fun i -> sr |> Seq.sumBy (fun j -> evenlyDiv (i,j)))
        |> (fun i -> i/2)
