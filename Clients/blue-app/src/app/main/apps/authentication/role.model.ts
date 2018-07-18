import { FuseUtils } from '@fuse/utils';

export class Role
{
    id: string;
    code: string;
    name: string;
    description: string;
    active: boolean;

    /**
     * Constructor
     *
     * @param role
     */
    constructor(role?)
    {
        role = role || {};
        this.id = role.id || '';
        this.code = role.code || '';
        this.name = role.name || '';
        this.description = role.description || '';
        this.active = role.active || false;
    }
}
