import { TestCallModel } from '@/Faculty/Backend/TestCont/TestCallModel';
import { TestResultModel } from '@/Faculty/Backend/TestCont/TestResultModel';

/** 테스트용 결과 모델 */
export interface TestCallResultModel 
{
    /** 개체 테스트 1 */
    Call?: TestCallModel,
    /** 개체 테스트 2 */
    Result?: TestResultModel,
}