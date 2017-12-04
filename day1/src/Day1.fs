namespace AdventOfCode2017

[<AutoOpen>]
module Day1 =
    let Halfwise (s:seq<int>) =
        let a = s |> Seq.toArray
        let jump = (Array.length a)/2
        let a2 = Array.append a (Array.take jump a)
        a
        |> Array.indexed
        |> Array.map (fun (i,j) -> (j,a2.[i+jump]) )
        |> Array.toSeq

    let CalcCaptcha (s:seq<int*int>) =
        s
        |> Seq.filter (fun (a,b) -> (a=b) )
        |> Seq.map (fun (a,_) -> a)
        |> Seq.sum