﻿namespace OfferPrice.Profile.Api.Models;

public class UpdateUserRequest
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

