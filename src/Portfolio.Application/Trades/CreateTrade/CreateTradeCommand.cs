using Portfolio.Domain.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Trades.CreateTrade
{
   public sealed record CreateTradeCommand(string Instrument, 
       int Quantity, decimal Price,
       Side Side, DateTime TradeDateUtc);
}
