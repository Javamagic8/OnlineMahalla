﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMahalla.Common.Model.Interface
{
    public partial interface IDataRepository
    {
        bool UserIsInRole(string ModuleName, int userID = 0);
    }
}
