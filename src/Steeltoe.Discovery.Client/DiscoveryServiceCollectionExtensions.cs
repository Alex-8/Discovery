﻿//
// Copyright 2015 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Steeltoe.Discovery.Client
{
    public static class DiscoveryServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscoveryClient(this IServiceCollection services, DiscoveryOptions discoveryOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (discoveryOptions == null)
            {
                throw new ArgumentNullException(nameof(discoveryOptions));
            }

            if (discoveryOptions.ClientType == DiscoveryClientType.UNKNOWN)
            {
                throw new ArgumentException("Client type UNKNOWN");
            }

            return DoAdd(services, discoveryOptions);
        }

        public static IServiceCollection AddDiscoveryClient(this IServiceCollection services, Action<DiscoveryOptions> setupOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupOptions == null)
            {
                throw new ArgumentNullException(nameof(setupOptions));
            }

            var options = new DiscoveryOptions();
            setupOptions(options);

            if (options.ClientType == DiscoveryClientType.UNKNOWN)
            {
                throw new ArgumentException("Client type UNKNOWN");
            }

            return DoAdd(services, options);
        }


        public static IServiceCollection AddDiscoveryClient(this IServiceCollection services, IConfiguration config)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            DiscoveryOptions configOptions = new DiscoveryOptions(config);
            return DoAdd(services, configOptions);

        }

        private static IServiceCollection DoAdd(IServiceCollection services, DiscoveryOptions config)
        {
            DiscoveryClientFactory factory = new DiscoveryClientFactory(config);
            services.Add(new ServiceDescriptor(typeof(IDiscoveryClient), factory.Create, ServiceLifetime.Singleton));
            return services;
        }

    }
}
