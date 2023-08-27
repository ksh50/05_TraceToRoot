public class Part
{
    public string Name { get; set; }
    public string PartNumber { get; set; }

    public Part(string name, string partNumber)
    {
        Name = name;
        PartNumber = partNumber;
    }

    public override string ToString()
    {
        return $"Part: {Name}, PartNumber: {PartNumber}";
    }
}

public class TreeNode<T>
{
    public T Data { get; set; }
    public TreeNode<T>? Parent { get; private set; }
    public List<TreeNode<T>> Children { get; set; }

    public TreeNode(T data)
    {
        Data = data;
        Children = new List<TreeNode<T>>();
    }

    public TreeNode<T> AddChild(T childData)
    {
        var childNode = new TreeNode<T>(childData);
        childNode.Parent = this; // Set this node as the parent of the child
        Children.Add(childNode);
        return childNode;
    }
}

public static class TreeUtility
{
    public static void PrintTree<T>(TreeNode<T> node, int indent = 0)
    {
        Console.WriteLine(new string(' ', indent * 2) + node.Data); // 2 spaces indent

        foreach (var child in node.Children)
        {
            PrintTree(child, indent + 1);
        }
    }

    public static void AddRandomChildren(TreeNode<Part> node, int depth, Random rng)
    {
        if (depth <= 0) return;

        int childCount = rng.Next(1, 4); // Each node can have 1 to 3 children
        for (int i = 0; i < childCount; i++)
        {
            var childPart = new Part($"Part_{depth}_{i}", $"P{depth}_{i}");
            node.AddChild(childPart); // Use AddChild method to add child node
            AddRandomChildren(node.Children.Last(), depth - 1, rng);
        }
    }
}

public class PartTreeSearch
{
    public static List<TreeNode<Part>> RecursiveSearch(TreeNode<Part> node, Part targetPart)
    {
        var results = new List<TreeNode<Part>>();

        if (node.Data.Name == targetPart.Name)
        {
            results.Add(node);
        }

        foreach (var childNode in node.Children)
        {
            var childResults = RecursiveSearch(childNode, targetPart);
            results.AddRange(childResults);
        }

        return results;
    }

    public static List<List<Part>> TraceToRoot(TreeNode<Part> rootNode, Part targetPart)
    {
        var allPaths = new List<List<Part>>();
        var matchingNodes = RecursiveSearch(rootNode, targetPart);

        foreach (var foundNode in matchingNodes)
        {
            var currentPath = new List<Part>();
            var currentNode = foundNode;

            while (currentNode != null)
            {
                currentPath.Insert(0, currentNode.Data); // Add to the beginning to maintain order
                currentNode = currentNode.Parent;
            }

            allPaths.Add(currentPath);
        }

        return allPaths;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var rootPart = new Part("RootPart", "RootPartNumber");
        var rootNode = new TreeNode<Part>(rootPart);

        Random rng = new Random();
        TreeUtility.AddRandomChildren(rootNode, 3, rng);

        TreeUtility.PrintTree(rootNode);

        Console.WriteLine("-----------------------------------");

        Part target = new Part("Part_2_0", "P2_0");
        var traceLists = PartTreeSearch.TraceToRoot(rootNode, target);
        foreach (var path in traceLists)
        {
            Console.WriteLine("----- Path -----");
            foreach (var part in path)
            {
                Console.WriteLine(part);
            }
        }
    }
}
