﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategoryInput
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public CreateCategoryInput(string name, string description, bool isActive)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }
}
