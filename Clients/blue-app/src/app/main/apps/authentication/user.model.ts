import { FuseUtils } from '@fuse/utils';

export class User
{
    id: string;
    name: string;
    description: string;
    active: boolean;

    /**
     * Constructor
     *
     * @param user
     */
    constructor(user?)
    {
        user = user || {};
        this.id = user.id || '';
        this.name = user.name || '';
        this.description = user.description || '';
        this.active = user.active || true;
    }
}
