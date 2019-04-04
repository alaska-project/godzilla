import { TestBed } from '@angular/core/testing';

import { DatabaseContextService } from './database-context.service';

describe('DatabaseContextService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DatabaseContextService = TestBed.get(DatabaseContextService);
    expect(service).toBeTruthy();
  });
});
