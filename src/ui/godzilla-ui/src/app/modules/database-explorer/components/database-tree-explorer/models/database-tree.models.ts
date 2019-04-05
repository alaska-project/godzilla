import { UiNodeReference } from '../../../clients/godzilla.clients';

export class EntityNode {
    constructor(
        public item: UiNodeReference, 
        public level = 1, 
        public expandable = false,
        public isLoading = false) { }
}
