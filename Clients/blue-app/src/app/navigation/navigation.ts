import { FuseNavigation } from '@fuse/types';

export const navigation: FuseNavigation[] = [
    {
        id       : 'applications',
        title    : 'Applications',
        translate: 'NAV.APPLICATIONS',
        type     : 'group',
        children : [
            {
                id       : 'home',
                title    : 'Home',
                translate: 'NAV.HOME.TITLE',
                type     : 'item',
                icon     : 'home',
                url      : '/home',
                badge    : {
                    title    : '25',
                    translate: 'NAV.HOME.BADGE',
                    bg       : '#F44336',
                    fg       : '#FFFFFF'
                }
            }
        ]
    },
    {
        id       : 'system',
        title    : 'System',
        translate: 'NAV.SYSTEM',
        type     : 'group',
        children : [
            {
                id          : 'users',
                title       : 'Users',
                translate   : 'NAV.USERS.TITLE',
                type        : 'item',
                icon        : 'person',
                url         : 'apps/authentication/users',
                exactMatch  : true
            },
            {
                id          : 'roles',
                title       : 'Roles',
                translate   : 'NAV.ROLES.TITLE',
                type        : 'item',
                icon        : 'supervisor_account',
                url         : 'apps/authentication/roles',
                exactMatch  : true
            }
        ]
    }
];
