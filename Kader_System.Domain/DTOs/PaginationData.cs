﻿namespace Kader_System.Domain.DTOs;

public class PaginationData<T> where T : class
{
    public int TotalRecords { get; set; } = 0;
    public List<T> Items { get; set; }  = [];
    //public int CurrentPage { get; set; }
    //public string FirstPageUrl { get; set; }
    //public int From { get; set; }
    //public int LastPage { get; set; }
    //public string LastPageUrl { get; set; }
    //public List<Link> Links { get; set; }
    //public string NextPageUrl { get; set; }
    //public string Path { get; set; }
    //public int PerPage { get; set; }
    //public object PreviousPage { get; set; }
    //public int To { get; set; }
    
}

public class Link
{
    public string url { get; set; }
    public string label { get; set; }
    public bool active { get; set; }
}