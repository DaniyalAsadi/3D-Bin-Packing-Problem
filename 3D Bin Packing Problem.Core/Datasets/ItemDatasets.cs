using _3D_Bin_Packing_Problem.Core.Model;
using System;
using System.Collections.Generic;

public static class ItemDatasets
{
    public static List<Item> Basic()
    {
        var orderId = Guid.NewGuid();

        return new List<Item>
        {
            Item.Create(10, 10, 10, 5,  orderId),                 // Small
            Item.Create(20, 10, 10, 8,  orderId),                 // Medium
            Item.Create(30, 20, 15, 15, orderId),                 // Large
            Item.Create(15, 15, 5,  4,  orderId),                 // Flat
            Item.Create(8,  8,  20, 6,  orderId)                  // Tall
        };
    }

    public static List<Item> WithFragileItems()
    {
        var orderId = Guid.NewGuid();

        return new()
        {
            Item.Create(10, 10, 10, 5, orderId),
            Item.Create(12, 12, 8,  4, orderId, isFragile: true, isStackable: false),
            Item.Create(15, 10, 10, 6, orderId),
            Item.Create(8,  8,  8,  3, orderId, isFragile: true, isStackable: false)
        };
    }

    public static List<Item> WithStackingRules()
    {
        var orderId = Guid.NewGuid();

        return new()
        {
            Item.Create(20, 20, 10, 10, orderId, isStackable: true,  maxLoadOnTop: 30),
            Item.Create(15, 15, 10, 8,  orderId, isStackable: true,  maxLoadOnTop: 20),
            Item.Create(10, 10, 10, 5,  orderId, isStackable: false),
            Item.Create(12, 12, 8,  6,  orderId, isStackable: false)
        };
    }

    public static List<Item> WithPrioritiesAndSequence()
    {
        var orderId = Guid.NewGuid();

        return new()
        {
            Item.Create(10, 10, 10, 5, orderId, priority: 1, loadingSequence: 1),
            Item.Create(20, 10, 10, 7, orderId, priority: 2, loadingSequence: 2),
            Item.Create(15, 15, 15, 9, orderId, priority: 3, loadingSequence: 3),
            Item.Create(8,  8,  8,  3, orderId, priority: 0, loadingSequence: 4)
        };
    }
    public static List<Item> GeneticStressTest(int count = 50)
    {
        var orderId = Guid.NewGuid();
        var random = new Random(42);

        var items = new List<Item>();

        for (int i = 0; i < count; i++)
        {
            var isFragile = random.NextDouble() < 0.2;
            var isStackable = !isFragile && random.NextDouble() > 0.3;

            items.Add(Item.Create(
                length: random.Next(5, 40),
                width: random.Next(5, 40),
                height: random.Next(5, 40),
                weight: random.Next(1, 30),
                orderId: orderId,
                isFragile: isFragile,
                isStackable: isStackable,
                maxLoadOnTop: isStackable ? random.Next(5, 50) : null,
                priority: random.Next(0, 5)
            ));
        }

        return items;
    }

}