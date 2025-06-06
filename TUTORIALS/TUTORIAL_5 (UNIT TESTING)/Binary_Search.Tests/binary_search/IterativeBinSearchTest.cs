using Binary_Search.binary_search;

namespace Binary_Search.Tests.binary_search;

public class IterativeBinSearchTest
{   
    [Fact]   
    public void ShouldFindIndexOfNumber() {
        //Arrange
        int[] Fibos = [1, 1, 2, 3, 5, 8, 13, 21, 34, 55];

        //Act
        var search3Result=IterativeBinSearch.Find(Fibos, 3);
        var search55Result=IterativeBinSearch.Find(Fibos, 55);

        //Assert
        Assert.Equal(3, search3Result);
        Assert.Equal(9, search55Result);
    }

    [Fact]    
    public void ShouldReturnNegativeInsertionPointWhenNotFound() {
        //Arrange
        int[] Fibos = [1, 1, 2, 3, 5, 8, 13, 21, 34, 55];

        //Act
        var search0Result=IterativeBinSearch.Find(Fibos, 0);
        var search4Result=IterativeBinSearch.Find(Fibos, 4);

        //Assert
        Assert.Equal(-1, search0Result);
        Assert.Equal(-5, search4Result);
    }

}