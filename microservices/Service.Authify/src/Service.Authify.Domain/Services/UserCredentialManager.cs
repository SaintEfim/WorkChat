﻿using AutoMapper;
using Service.Authify.Data.Repository;
using Service.Authify.Domain.Exceptions;
using Service.Authify.Domain.Models;
using Service.Authify.Domain.Models.Requests;
using Service.Authify.Domain.Models.Responses;

namespace Service.Authify.Domain.Services;

public class UserCredentialManager : IUserCredentialManager
{
    private readonly IUserCredentialRepository _repository;
    private readonly IMapper _mapper;

    public UserCredentialManager(IUserCredentialRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Register(RegistrationRequest registrationRequest, CancellationToken cancellationToken = default)
    {
        if (_repository.IsUniqueUser(registrationRequest.Email))
        {
            throw new DuplicateUserException(
                $"A user with the same {registrationRequest.Email} address already exists.");
        }

        var user = _mapper.Map<UserCredential>(registrationRequest);

        if (user == null)
        {
            throw new MappingFailedException("Failed to map RegistrationRequest to UserCredential.");
        }

        await _repository.Register(user, cancellationToken);
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetUserByEmailAndPasswordAsync(loginRequest, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        var loginResponse = await _repository.Login(user, cancellationToken);
        if (loginResponse == null)
        {
            throw new DataNotFoundException("Login response is null.");
        }

        return loginResponse;
    }
}