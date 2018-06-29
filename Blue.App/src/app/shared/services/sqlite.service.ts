import { Injectable } from '@angular/core';
import { SQLite, SQLiteObject } from '@ionic-native/sqlite';

@Injectable()
export class SqliteService {
    private sqliteObject: SQLiteObject;

    constructor(
        private sqlite: SQLite,
    ){}

    initialize(callback){
        this.sqliteObject.openDBs({
            name: 'data.db',
            location: 'default',
            iosDatabaseLocation: 'Library'
        }).then((db: SQLiteObject) => {
            db.executeSql('create table danceMoves(name VARCHAR(32))', {})
            .then(() => console.log('Executed SQL'))
            .catch(e => {
                console.log(e)
            });
        })
        .catch(e => {
            console.log(e)
        });
    }

    createDatabase(){
        this.sqlite.create({
            name: 'data.db',
            location: 'default',
            iosDatabaseLocation: 'Library'
        }).then((db: SQLiteObject) => {
            db.executeSql('create table danceMoves(name VARCHAR(32))', {})
            .then(() => console.log('Executed SQL'))
            .catch(e => console.log(e));
        })
        .catch(e => console.log(e));
    }

    executeSql(sqlString: string){

    }
}