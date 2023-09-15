using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Exceptions;
using MySuperShop.Domain.Repositories;
using MySuperShop.Domain.Services;
using MySuperShop.HttpModels.Requests;
using MySuperShop.HttpModels.Responses;
#pragma warning disable CS8604

namespace MyShopBackend.Controllers;


[Route("cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;
    private readonly IRepository<Product> _repository;

    public CartController(CartService cartService, IRepository<Product> repository)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [Authorize]
    [HttpGet("current")]
    public async Task<ActionResult<CartResponse>> GetCurrentCart(CancellationToken cancellationToken)
    {
        try
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guid = Guid.Parse(strId!);
            var cart = await _cartService.GetAccountCart(guid, cancellationToken);
            var response = await MakeCartResponse(cart, _repository);
            return response;
        }
        catch (Exception)
        {
            return Conflict(new ErrorResponse("Не найдено корзины для данного аккаунта!"));
        }
    }

    //    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult> AddCartItem(
        [FromBody] AddCartItemRequest reqest,
        CancellationToken cancellationToken)
    {
        try
        {
            await _cartService.AddProduct(reqest.AccountId, reqest.ProductId, reqest.Quantity, cancellationToken);
            return Ok();
        }
        catch (CartNotFoundException)
        {
            return Conflict(new ErrorResponse("Не найдено корзины!"));
        }
    }


    private async Task<CartResponse> MakeCartResponse(Cart cart, IRepository<Product> repository)
    {
        if (cart == null) throw new ArgumentNullException(nameof(cart));
    
        if (cart.Items.IsNullOrEmpty())
            return new CartResponse();
    
        var response = new CartResponse();
        foreach (var it in cart.Items!)
        {
            var product = await repository.GetById(it.ProductId);
            response.Items.Add(new ItemResponse(it.Id, product.Name, it.Quantity));
        }
        return response;
    }
}