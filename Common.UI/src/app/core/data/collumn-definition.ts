export interface CollumnDefinition {
     id: string;
     field: string;
     header: string;
     type?: CollumnType;
     icon?: string;
     action?: (data: any) => void;
}

export enum CollumnType {
    Data,
    Action
}
