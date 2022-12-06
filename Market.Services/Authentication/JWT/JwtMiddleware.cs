﻿using System;
using Microsoft.AspNetCore.Http;

namespace Market.Services.Authentication.JWT
{
	public class JwtMiddleware
	{
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.get(userId.Value);
            }

            await _next(context);
        }
    }
}
