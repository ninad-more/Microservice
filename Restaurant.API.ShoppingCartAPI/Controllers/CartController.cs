﻿using Microsoft.AspNetCore.Mvc;
using Restaurant.API.ShoppingCartAPI.Messages;
using Restaurant.API.ShoppingCartAPI.Models.Dto;
using Restaurant.API.ShoppingCartAPI.Repository.Interfaces;
using Restaurant.MessageBus;

namespace Restaurant.API.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        protected Responsedto _response;
        private ICartRepository _repository;
        private IMessageBus _messageBus;
        public CartController(ICartRepository cartRepository, IMessageBus messageBus)
        {
            _repository = cartRepository;
            this._response = new Responsedto();
            _messageBus = messageBus;
        }

        [HttpGet("GetCart")]
        public async Task<object> GetCart()
        {
            try
            {
                CartDto cartDto = await _repository.GetCart();
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _repository.CreateUpdateCart(cartDto);
                _response.Result = cartDt;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _repository.CreateUpdateCart(cartDto);
                _response.Result = cartDt;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartId)
        {
            try
            {
                bool isSuccess = await _repository.RemoveFromCart(cartId);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDto checkoutHeader)
        {
            try
            {
                CartDto cartDto = await _repository.GetCart();
                if (cartDto == null)
                {
                    return BadRequest();
                }               

                checkoutHeader.CartDetails = cartDto.CartDetails;

                await _messageBus.PublishMessage(checkoutHeader, "checkoutqueue");

                await _repository.ClearCart();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
