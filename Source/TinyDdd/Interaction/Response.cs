﻿using System.Collections.Generic;
using System.Linq;
using SwissKnife.Collections;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Interaction
{
    public class Response<T> : Response
    {
        public T Result { get; private set; }

        public Response(T result)
        {
            Result = result;
        }

        public Response()
        {
            Result = default(T);
        }

        public static Response<T> From(Response response)
        {
            return From(response, default(T));
        }

        public static Response<T> From(Response response, T result)
        {
            Argument.IsNotNull(response, "response");

            Response<T> newResponse = new Response<T>(result);
            newResponse.Add(response);
            return newResponse;
        }
    }

    public class Response
    {
        private readonly IList<ResponseMessage> _responseMessages = new List<ResponseMessage>();

        public bool HasInformations { get { return HasMessagesOfType( MessageType.Information ); } }
        public bool HasWarnings { get { return HasMessagesOfType( MessageType.Warning ); } }
        public bool HasErrors { get { return HasMessagesOfType( MessageType.Error ); } }

        public IEnumerable<ResponseMessage> Informations { get { return GetMessagesOfType( MessageType.Information ); } }
        public IEnumerable<ResponseMessage> Warnings { get { return GetMessagesOfType( MessageType.Warning ); } }
        public IEnumerable<ResponseMessage> Errors { get { return GetMessagesOfType( MessageType.Error ); } }
        public IEnumerable<ResponseMessage> TechnicalErrors { get { return GetMessagesOfType(MessageType.TechnicalError); } }
        public IEnumerable<ResponseMessage> AllMessages { get { return _responseMessages; } }

        public void Add(ResponseMessage responseMessage)
        {
            Argument.IsNotNull( responseMessage, "responseMessage" );

            _responseMessages.Add( responseMessage );
        }

        public Response Add(Response response)
        {
            Argument.IsNotNull( response, "response" );
            Argument.IsValid( response != this, string.Format( "{0} can not be added to itself.", typeof( Response ) ), "response" );

            _responseMessages.AddMany(response._responseMessages);

            return this;
        }

        public Response AddInformation(string message)
        {
            AddInformation(string.Empty, message);

            return this;
        }

        public Response AddInformation(string key, string message)
        {
            _responseMessages.Add( new ResponseMessage( MessageType.Information, key, message ) );

            return this;
        }

        public Response AddWarning(string message)
        {
            AddWarning(string.Empty, message);

            return this;
        }

        public Response AddWarning(string key, string message)
        {
            _responseMessages.Add( new ResponseMessage( MessageType.Warning, key, message ) );

            return this;
        }

        public Response AddError(string message)
        {
            AddError(string.Empty, message);

            return this;
        }

        public Response AddError(string key, string message)
        {
            _responseMessages.Add( new ResponseMessage( MessageType.Error, key, message ) );

            return this;
        }

        public Response AddTechnicalError(string key, string message)
        {
            _responseMessages.Add(new ResponseMessage(MessageType.TechnicalError, key, message));

            return this;
        }

        public Response AddErrors(IEnumerable<string> errors)
        {
            Argument.IsNotNull( errors, "errors" );

            _responseMessages.AddMany( errors.Select( error => new ResponseMessage( MessageType.Error, string.Empty, error ) ) );

            return this;
        }

        public Response InsertError(string key, string message)
        {
            _responseMessages.Insert( 0, new ResponseMessage( MessageType.Error, key, message ) );

            return this;
        }

        private bool HasMessagesOfType(MessageType messageType)
        {
            return _responseMessages.Any( x => x.MessageType == messageType );
        }

        private IEnumerable<ResponseMessage> GetMessagesOfType(MessageType messageType)
        {
            return _responseMessages.Where( x => x.MessageType == messageType ).ToList().AsReadOnly();
        }
    }
}
