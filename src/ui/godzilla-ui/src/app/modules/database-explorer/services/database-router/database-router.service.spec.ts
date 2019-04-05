import { TestBed } from '@angular/core/testing';

import { DatabaseRouterService } from './database-router.service';

describe('DatabaseRouterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DatabaseRouterService = TestBed.get(DatabaseRouterService);
    expect(service).toBeTruthy();
  });
});
