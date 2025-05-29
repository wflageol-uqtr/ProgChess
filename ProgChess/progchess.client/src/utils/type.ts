
export type UnitTest = {
  id: number;
  code: string;
  isActive: boolean;
};

export type Exercise = {
  id: number;
  situation: string;
  baseCode: string;
  unitTests: UnitTest[];
  studentCodes: string[]
};

export type TestResult = {
  $type: string,
  testName: string,
  success: boolean,
  expected?: string,
  actual?: string
}