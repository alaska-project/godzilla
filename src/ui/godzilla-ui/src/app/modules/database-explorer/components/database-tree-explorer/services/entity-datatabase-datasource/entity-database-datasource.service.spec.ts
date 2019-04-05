import { TestBed } from '@angular/core/testing';

import { EntityDatabaseDatasourceService } from './entity-database-datasource.service';

describe('EntityDatabaseDatasourceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EntityDatabaseDatasourceService = TestBed.get(EntityDatabaseDatasourceService);
    expect(service).toBeTruthy();
  });
});
