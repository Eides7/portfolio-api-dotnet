using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Portfolio.GetPositions
{
   public sealed record PositionDto(string Instrument, int NetQuantity);
}
