import { EndpointService } from '../services/endpoint/endpoint.service';

export abstract class ClientBase {

    constructor(private configuration: EndpointService) {
        this.configuration.endpointChanged().subscribe(url => {
            const client = <any>this;
            client.baseUrl = url;
            console.log('client base endpoint changed', this);
        });
    }

    getBaseUrl(value: string) {
        return this.configuration.getEndpoint();
    }
}
