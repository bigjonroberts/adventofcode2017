module Tests

open System
open Xunit
open AdventOfCode2017

[<Theory>]
[<InlineData("5\t1\t9\t5",8)>]
[<InlineData("7\t5\t3",4)>]
[<InlineData("2\t4\t6\t8",6)>]
let ``span test`` (input:String, output:int) =
    let actual =
        input.Split('\t')
        |> Seq.map (fun (s:String) -> Convert.ToInt32(s))
        |> span
    Assert.Equal(output,actual)
    actual

[<Fact>]
let ``Solution 1 test`` () =
    let input =
        seq { 
            yield List.toSeq [5;1;9;5]
            yield List.toSeq [7;5;3]
            yield List.toSeq [2;4;6;8]
        }
    Assert.Equal(18,Seq.sumBy span input)

[<Theory>]
[<InlineData("5\t8",0)>]
[<InlineData("5\t2",0)>]
[<InlineData("5\t9",0)>]
[<InlineData("5\t5",0)>]
[<InlineData("9\t8",0)>]
[<InlineData("9\t2",0)>]
[<InlineData("9\t9",0)>]
[<InlineData("9\t5",0)>]
let ``evenlyDiv test`` (input:String, output:int) =
    let actual =
        input.Split('\t')
        |> Seq.map (fun (s:String) -> Convert.ToInt32(s))
        |> Seq.pairwise |> Seq.head
        |> evenlyDiv
    Assert.Equal(output,actual)
    actual


[<Theory>]
[<InlineData("5\t9\t2\t8",4)>]
//[<InlineData("7\t5\t3",4)>]
//[<InlineData("2\t4\t6\t8",6)>]
let ``div2 test`` (input:String, output:int) =
    let actual =
        input.Split('\t')
        |> Seq.map (fun (s:String) -> Convert.ToInt32(s))
        |> div2
    Assert.Equal(output,actual)
    actual


// let halfwiseHelper input =
//     input
//     |> List.toSeq
//     |> Halfwise
//     |> Seq.toList

// [<Fact>]
// let ``Halfwise a`` () =
//     Assert.Equal<list<int*int>>([(1,1);(2,2);(1,1);(2,2)], halfwiseHelper [1;2;1;2;])

// [<Fact>]
// let ``Halfwise b`` () =
//     Assert.Equal<list<int*int>>([(1,2);(2,1);(2,1);(1,2)], halfwiseHelper [1;2;2;1;])

// [<Fact>]
// let ``CalcCaptcha 2 a`` () =
//     Assert.Equal(6, [(1,1);(2,2);(1,1);(2,2)] |> List.toSeq |> CalcCaptcha)

// [<Theory>]
// [<InlineData("1212",6)>]
// [<InlineData("1221",0)>]
// [<InlineData("123425",4)>]
// [<InlineData("123123",12)>]
// [<InlineData("12131415",4)>]
// let ``Solution 2 test`` (input:seq<char>, output:int) =
//     let actual =
//         input 
//         |> Seq.map (Char.GetNumericValue >> int) 
//         |> Halfwise
//         |> CalcCaptcha 
//     Assert.Equal(output,actual)
//     actual
