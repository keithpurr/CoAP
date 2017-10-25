﻿using System;
using Com.AugustCellars.CoAP.Server.Resources;


    /// <summary>
    /// This resource responds with a kind "hello world" to GET requests.
    /// </summary>
    class HelloWorldResource : Resource
    {
        public HelloWorldResource(String name)
            : base(name)
        {
            Attributes.Title = "GET a friendly greeting!";
            Attributes.AddResourceType("HelloWorldDisplayer");
        }

        protected override void DoGet(CoapExchange exchange)
        {
            exchange.Respond("Hello World!");
        }
    }

