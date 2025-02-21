namespace _3D_Bin_Packing_Problem.Models;

public class BoxPlacement
{
    public Box Box { get; set; }
    public List<PlacedProduct> PlacedProducts { get; set; } = new List<PlacedProduct>();
    private List<FreeSpace> FreeSpaces { get; set; } = new List<FreeSpace>();

    public BoxPlacement(Box box)
    {
        Box = box;
        FreeSpaces.Add(new FreeSpace(0, 0, 0, box.Length, box.Height, box.Width)); // Initialize with full box as free space
    }

    public override string ToString() => $"Box {Box.Id} ({Box.Length}x{Box.Height}x{Box.Width})";

    public List<FreeSpace> GetFreeSpaces() => FreeSpaces;

    public bool TryPlaceProduct(Gene gene)
    {
        var product = gene.Product;

        for (int i = 0; i < FreeSpaces.Count; i++)
        {
            var space = FreeSpaces[i];

            // Check if product fits within this free space
            if (product.Length <= space.Length && product.Height <= space.Height && product.Width <= space.Width)
            {
                var placedProduct = new PlacedProduct(gene, space.X, space.Y, space.Z);
                PlacedProducts.Add(placedProduct);

                // Remove the used free space and split remaining
                FreeSpaces.RemoveAt(i);
                FreeSpaces.AddRange(SplitSpace(space, new FreeSpace(space.X, space.Y, space.Z, product.Length, product.Height, product.Width)));

                return true;
            }
        }

        return false; // No space found
    }

    private bool IsOverlapping(FreeSpace space, FreeSpace occupied)
    {
        return !(occupied.X + occupied.Length <= space.X || space.X + space.Length <= occupied.X ||
                 occupied.Y + occupied.Height <= space.Y || space.Y + space.Height <= occupied.Y ||
                 occupied.Z + occupied.Width <= space.Z || space.Z + space.Width <= occupied.Z);
    }

    private List<FreeSpace> SplitSpace(FreeSpace space, FreeSpace occupied)
    {
        List<FreeSpace> newSpaces = new List<FreeSpace>();

        // Remaining space on X-axis
        if (occupied.X + occupied.Length < space.X + space.Length)
            newSpaces.Add(new FreeSpace(occupied.X + occupied.Length, space.Y, space.Z,
                                        space.Length - occupied.Length, space.Height, space.Width));

        // Remaining space on Y-axis
        if (occupied.Y + occupied.Height < space.Y + space.Height)
            newSpaces.Add(new FreeSpace(space.X, occupied.Y + occupied.Height, space.Z,
                                        space.Length, space.Height - occupied.Height, space.Width));

        // Remaining space on Z-axis
        if (occupied.Z + occupied.Width < space.Z + space.Width)
            newSpaces.Add(new FreeSpace(space.X, space.Y, occupied.Z + occupied.Width,
                                        space.Length, space.Height, space.Width - occupied.Width));

        return newSpaces;
    }

    public class FreeSpace
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public FreeSpace(int x, int y, int z, int length, int height, int width)
        {
            X = x;
            Y = y;
            Z = z;
            Length = length;
            Height = height;
            Width = width;
        }

        public override string ToString() => $"Free space ({Length}x{Height}x{Width}) at ({X},{Y},{Z})";
    }
}

