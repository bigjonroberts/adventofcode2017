namespace AdventOfCode2017

[<AutoOpen>]
module Day1 =
    let CalcCaptcha (s:seq<int>) =
        s
        |> Seq.pairwise 
        |> Seq.filter (fun (a,b) -> (a=b) )
        |> Seq.map (fun (a,_) -> a)
        |> Seq.sum