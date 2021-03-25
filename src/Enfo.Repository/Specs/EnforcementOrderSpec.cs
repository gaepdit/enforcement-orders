﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Enfo.Repository.Resources;

namespace Enfo.Repository.Specs
{
    public class EnforcementOrderSpec
    {
        [DisplayName("Facility")]
        public string FacilityFilter { get; set; }

        public string County { get; set; }

        [DisplayName("Order Number")]
        public string OrderNumber { get; set; }

        [DisplayName("Legal Authority")]
        public int? LegalAuthId { get; set; }

        [DisplayName("Beginning Date")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [DisplayName("Ending Date")]
        [DisplayFormat(DataFormatString = DisplayFormats.FormatDateEdit, ApplyFormatInEditMode = true)]
        public DateTime? TillDate { get; set; }

        [DisplayName("Enforcement Order Status")]
        public ActivityState Status { get; set; } = ActivityState.All;

        public OrderSorting SortOrder { get; set; } = OrderSorting.DateDesc;

        public EnforcementOrderSpec TrimAll()
        {
            County = County?.Trim();
            FacilityFilter = FacilityFilter?.Trim();
            OrderNumber = OrderNumber?.Trim();
            return this;
        }

        public IDictionary<string, string> AsRouteValues => new Dictionary<string, string>()
        {
            {nameof(FacilityFilter), FacilityFilter},
            {nameof(County), County},
            {nameof(LegalAuthId), LegalAuthId?.ToString()},
            {nameof(FromDate), FromDate?.ToString()},
            {nameof(TillDate), TillDate?.ToString()},
            {nameof(Status), Status.ToString()},
            {nameof(OrderNumber), OrderNumber},
            {nameof(SortOrder), SortOrder.ToString()},
        };
    }
}