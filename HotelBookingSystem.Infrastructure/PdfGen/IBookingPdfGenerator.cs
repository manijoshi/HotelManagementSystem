using HotelBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Infrastructure.PdfGen
{
    public interface IBookingPdfGenerator : IPdfGenerator<Booking>
    {
    }
}
