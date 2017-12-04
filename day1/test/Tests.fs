module Tests

open System
open Xunit
open AdventOfCode2017

[<Theory>]
[<InlineData("1122",3L)>]
[<InlineData("1111",4L)>]
[<InlineData("1234",0L)>]
[<InlineData("91212129",9L)>]
let ``Solution 1 test`` (input:seq<char>, output:int64) =
    let actual =
        let ints = input |> Seq.map (Char.GetNumericValue >> int) 
        Seq.head ints |> Seq.singleton |> Seq.append ints         
        |> Seq.pairwise
        |> CalcCaptcha 
    Assert.Equal(output,actual)
    actual

let halfwiseHelper input =
    input
    |> List.toSeq
    |> Halfwise
    |> Seq.toList

[<Fact>]
let ``Halfwise a`` () =
    Assert.Equal<list<int*int>>([(1,1);(2,2);(1,1);(2,2)], halfwiseHelper [1;2;1;2;])

[<Fact>]
let ``Halfwise b`` () =
    Assert.Equal<list<int*int>>([(1,2);(2,1);(2,1);(1,2)], halfwiseHelper [1;2;2;1;])

[<Fact>]
let ``CalcCaptcha 2 a`` () =
    Assert.Equal(6L, [(1,1);(2,2);(1,1);(2,2)] |> List.toSeq |> CalcCaptcha)

[<Theory>]
[<InlineData("1212",6L)>]
[<InlineData("1221",0L)>]
[<InlineData("123425",4L)>]
[<InlineData("123123",12L)>]
[<InlineData("12131415",4L)>]
let ``Solution 2 test`` (input:seq<char>, output:int64) =
    let actual =
        input 
        |> Seq.map (Char.GetNumericValue >> int) 
        |> Halfwise
        |> CalcCaptcha 
    Assert.Equal(output,actual)
    actual
