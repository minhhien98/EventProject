﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IBackGroundTaskService
    {
        Task CheckIn(CancellationToken cancellationToken);
    }
}
