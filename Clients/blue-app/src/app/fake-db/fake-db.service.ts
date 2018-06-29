import { InMemoryDbService } from 'angular-in-memory-web-api';
import { AuthenticationFakeDb } from './authentication';

export class FakeDbService implements InMemoryDbService
{
    createDb(): any
    {
        return {
            'users' : AuthenticationFakeDb.users,
            'roles' : AuthenticationFakeDb.roles
        };
    }
}
