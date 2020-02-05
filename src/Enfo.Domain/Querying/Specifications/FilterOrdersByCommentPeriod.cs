﻿using System;
using Enfo.Domain.Entities;

namespace Enfo.Domain.Querying
{
    public class FilterOrdersByCommentPeriod : Specification<EnforcementOrder>
    {
        public FilterOrdersByCommentPeriod(DateTime commentPeriodClosesAfter)
        {
            ApplyCriteria(e => e.CommentPeriodClosesDate >= commentPeriodClosesAfter);
        }
    }
}
