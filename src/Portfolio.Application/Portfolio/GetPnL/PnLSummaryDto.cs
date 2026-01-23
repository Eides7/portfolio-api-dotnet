using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Portfolio.GetPnL
{
    public sealed record PnLSummaryDto(decimal TotalNetCashFlow, IReadOnlyList<PnLByInstrumentDto> ByInstrument);
    
}
