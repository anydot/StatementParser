﻿using System.Collections.Generic;

namespace ExchangeRateProvider.Models
{
	internal class CurrencyList : ICurrencyList
	{
		private readonly Dictionary<string, CurrencyDescriptor> currencyList = new Dictionary<string, CurrencyDescriptor>();

		public CurrencyDescriptor this[string currencyCode] => currencyList[currencyCode.ToLower()];

		public bool IsEmpty => currencyList.Count == 0;

        public CurrencyList(IList<CurrencyDescriptor> currencyList)
		{
			foreach (var currencyDescriptor in currencyList)
			{
				this.currencyList.Add(currencyDescriptor.Code.ToLower(), currencyDescriptor);
			}
		}
	}
}
