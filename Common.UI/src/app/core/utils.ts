export module Utils {
    interface EnumType { [key: number]: string; }

    export function getModuleId(__moduleName: string): string {
        return __moduleName.replace('/build', '');
    }

    export function newGUID(): string {
        let d = new Date().getTime();
        if (window.performance && typeof window.performance.now === 'function') {
            d += performance.now(); // use high-precision timer if available
        }
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
            // tslint:disable-next-line:no-bitwise
            const r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            // tslint:disable-next-line:no-bitwise
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    }

    function getEnumElementsAsArray(enumParam: EnumType): (string | number)[] {
        return Object.keys(enumParam).map(s => enumParam[s]);
    }

    export function getEnumStrings(enumParam: EnumType): string[] {
        return getEnumElementsAsArray(enumParam).filter(s => typeof s === 'string') as string[];
    }

    export function getEnumNumbers(enumParam: EnumType): number[] {
        return getEnumElementsAsArray(enumParam).filter(s => typeof s === 'number') as number[];
    }

    export function firstOrDefault(collection: any[], defaultValue: any): any {
        for (const i in collection) {
            if (collection.hasOwnProperty(i)) {
                return collection[i];
            }
        }
        return defaultValue;
    }
}
