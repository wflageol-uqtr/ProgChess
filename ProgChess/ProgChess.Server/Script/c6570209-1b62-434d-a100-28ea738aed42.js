import assert from 'node:assert/strict';
import { it } from 'node:test';
function add1Plus1() {
  return 1+2;
}
it('should equal one', () => {
  assert.equal(add1Plus1(), 2)
});
it('should equal two', () => {
  assert.equal(add1Plus1(), 3)
});