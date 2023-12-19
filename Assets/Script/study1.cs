using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class study1 : MonoBehaviour
{
    public struct Item: IEquatable<Item>
    {
        public uint Number;
        public string Color;
        public double Price;
        public int Value;

        public bool Equals(Item other)
        {
                return Number == other.Number && Color == other.Color && Price == other.Price && Value == other.Value;
        }
    }

    List<Item> list;

    void Awake() 
    {   
       list = ChangeValue();
    }

    void Start()
    {
        Debug.Log("-------------------기본값------------------------");
        PrintItems(list);

        Debug.Log("-------------------작업 1------------------------");
        Variable(list);
        PrintItems(list);

        Debug.Log("-------------------작업 2------------------------");
        Debug.Log("색상별 정렬");
        SortColor(list);
        PrintItems(list);

        Debug.Log("가격별 정렬");
        SortPrice(list);
        PrintItems(list);

        Debug.Log("개수별 정렬"); 
        SortValue(list);
        PrintItems(list);

        Debug.Log("-------------------작업 3------------------------");
        Average(list);

        Debug.Log("-------------------작업 4------------------------");
        Copy(list);

    }


    List<Item> ChangeValue()
    {
        List<Item> item = new List<Item>();

        for(uint i = 1; i <= 10; i++)
        {
            Item newItem = new Item
            {
                Number = i,
                Color = GetRandomColor(),
                Price = UnityEngine.Random.Range(50,801),
                Value = UnityEngine.Random.Range(2,11)
            };

            item.Add(newItem);
        }

        return item;
    }

    string GetRandomColor()
    {
        string[] colors = { "red", "orange", "yellow", "green", "blue", "indigo", "violet" };
        return colors[UnityEngine.Random.Range(0, 7)];
    }

    void PrintItems(List<Item> items)
    {
        foreach (var item in items)
        {
            Debug.Log($"item {item.Number} = color: {item.Color}, price: {item.Price:F2}, Value: {item.Value}");
        }
    }

    void Variable(List<Item> items)
    {
        List<Item> modifiedItems = new List<Item>();
        List<Item> modifiedItems2 = new List<Item>();

        double[] result = new double[10];

        for (int i = 0; i < items.Count; i++)
        {
            result[i] = items[i].Value * items[i].Price;

            Item newItem = new Item
            {
                Number = items[i].Number,
                Color = items[i].Color,
                Price = items[i].Price,
                Value = items[i].Value - 1
            };
            modifiedItems.Add(newItem);
        }

        for (int i = 0; i < items.Count; i++)
        {
            Item newItem = new Item
            {
                Number = modifiedItems[i].Number,
                Color = modifiedItems[i].Color,
                Price = result[i] / modifiedItems[i].Value,
                Value = modifiedItems[i].Value
            };
            modifiedItems2.Add(newItem);
        }

        items.Clear();
        items.AddRange(modifiedItems2);
    }

    void SortColor(List<Item> items)
    {
        Dictionary<string, int> colorOrder = new Dictionary<string, int>
        {
            {"red", 0},
            {"orange", 1},
            {"yellow", 2},
            {"green", 3},
            {"blue", 4},
            {"indigo", 5},
            {"violet", 6}
        };
        items.Sort((a, b) => colorOrder[a.Color].CompareTo(colorOrder[b.Color]));
    }

    void SortPrice(List<Item> items)
    {
        items.Sort((a, b) => a.Price.CompareTo(b.Price));
    }

    void SortValue(List<Item> items)
    {
        items.Sort((a, b) => a.Value.CompareTo(b.Value));
    }

    void Average(List<Item> items)
    {
        var colorGroups = items.GroupBy(item => item.Color);

        foreach(var group in colorGroups)
        {
            double averagePrice = group.Average(item => item.Price);
            Debug.Log($"Color: {group.Key}, Average Price: {averagePrice:F2}");
        }
    }

    void Copy(List<Item> items)
    {
        if(items.Count > 3)
        {
            Item duplicatedItem = items[3];

            items.Insert(3, duplicatedItem);

            int indexOfDuplicatedItem = items.IndexOf(duplicatedItem);
            Debug.Log($"\n복제한 항목의 인덱스 확인: {indexOfDuplicatedItem}");
        }
        else
        {
            Debug.Log("\n복제 작업을 위한 충분한 아이템이 없습니다.");
        }
    }
}
