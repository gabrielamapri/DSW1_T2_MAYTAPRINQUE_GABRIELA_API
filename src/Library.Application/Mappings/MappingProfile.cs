using AutoMapper;
using Library.Application.DTOs.Book;
using Library.Application.DTOs.Loan;
using Library.Domain.Entities;

namespace Library.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();

            CreateMap<Loan, LoanDto>()
                .ForMember(d => d.BookTitle, o => o.MapFrom(s => s.Book != null ? s.Book.Title : string.Empty));
            CreateMap<CreateLoanDto, Loan>();
            CreateMap<UpdateLoanDto, Loan>();
        }
    }
}
