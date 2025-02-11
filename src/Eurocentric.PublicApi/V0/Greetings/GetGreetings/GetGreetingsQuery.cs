using Eurocentric.PublicApi.V0.Greetings.Common;
using MediatR;

namespace Eurocentric.PublicApi.V0.Greetings.GetGreetings;

public sealed record GetGreetingsQuery(int Quantity, Language Language) : IRequest<GetGreetingsResponse>;
