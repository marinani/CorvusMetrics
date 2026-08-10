using Corvus.Application.Common.Results;
using Corvus.Application.Dtos;
using MediatR;

namespace Corvus.Application.Authentication.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;