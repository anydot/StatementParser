﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASoft.TextDeserializer;
using ASoft.TextDeserializer.Exceptions;
using StatementParser.Models;
using StatementParser.Parsers.Brokers.MorganStanley.PdfModels;

namespace StatementParser.Parsers.Brokers.MorganStanley
{
	internal class MorganStanleyStatementPdfParser : ITransactionParser
	{
		private bool CanParse(string statementFilePath)
        {
            return File.Exists(statementFilePath) && Path.GetExtension(statementFilePath).ToLowerInvariant() == ".pdf";
        }

		public IList<Transaction> Parse(string statementFilePath)
		{
			if (!CanParse(statementFilePath))
			{
				return null;
			}

			var transactions = new List<Transaction>();

			using (var textSource = new TextSource(statementFilePath))
			{
				try
				{
					var parsedDocument = new TextDocumentParser<StatementModel>().Parse(textSource);

					transactions.AddRange(GetTransactions(parsedDocument));
				}
				catch (TextException)
				{
					return null;
				}
			}

			return transactions;
		}

		private decimal SearchForTax(StatementModel statementModel, TransactionModel transactionModel)
		{
			return statementModel.Transactions
				.Where(i => i.Type == "Withholding Tax" && i.Date == transactionModel.Date)
				.Select(i => i.NetAmount).FirstOrDefault();
		}

		private IEnumerable<Transaction> GetTransactions(StatementModel statementModel)
		{
			var output = new List<Transaction>();

			output.AddRange(statementModel.Transactions
				.Where(i => i.Type == "Share Deposit")
				.Select(i => new DepositTransaction(Broker.MorganStanley, i.Date, statementModel.Name, i.Quantity, i.Price, Currency.USD)));

			output.AddRange(statementModel.Transactions
				.Where(i => i.Type == "Dividend Credit")
				.Select(i => new DividendTransaction(Broker.MorganStanley, i.Date, statementModel.Name, i.GrossAmount, SearchForTax(statementModel, i), Currency.USD)));

			return output;
		}
	}
}
