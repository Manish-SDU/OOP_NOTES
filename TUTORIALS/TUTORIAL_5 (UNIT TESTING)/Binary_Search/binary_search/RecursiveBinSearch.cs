namespace Binary_Search.binary_search;

public class RecursiveBinSearch
{
    public static int RecursiveFind(int[] list, int key) {
        int low = 0;
        int high = list.Length - 1;
        return RecursiveFind(list, key, low, high);
    }

    private static int RecursiveFind(int[] list, int key,
        int low, int high) {
        if (low > high) // The list has been exhausted without a match
        {
            return -low - 1;
        }

        int mid = (low + high) / 2;
        
        if (key < list[mid]) {
            return RecursiveFind(list, key, low, mid - 1);
        }

        if (key == list[mid]) {
            return mid;
        }

        return RecursiveFind(list, key, mid + 1, high);
    }

}