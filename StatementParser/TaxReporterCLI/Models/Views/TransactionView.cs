using StatementParser.Attributes;
using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class TransactionView
	{
		public Transaction Transaction { get; }

		[Description("Exchange rate per Day")]
		public decimal? ExchangeRatePerDay { get; }

		[Description("Exchange rate per Year")]
		public decimal? ExchangeRatePerYear { get; }

		[Description("Exchanged to currency")]
        public Currency ExchangedToCurrency { get; }

        public TransactionView(Transaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear, Currency exchangedToCurrency)
		{
			this.Transaction = transaction;
			this.ExchangeRatePerDay = exchangeRatePerDay;
			this.ExchangeRatePerYear = exchangeRatePerYear;
            ExchangedToCurrency = exchangedToCurrency;
        }

		public override string ToString()
		{
			return $"{Transaction} {nameof(ExchangedToCurrency)}:{ExchangedToCurrency} {nameof(ExchangeRatePerDay)}: {ExchangeRatePerDay} {nameof(ExchangeRatePerYear)}: {ExchangeRatePerYear}";
		}
	}
}