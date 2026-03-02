# Claude Code Guidelines

## Workflow Process

### 0. Reference Documentation First
- Check relevant markdown files in `docs/` folder before relying on session context
- Existing documentation may contain accurate, up-to-date information about the codebase
- Use documented knowledge as the foundation; supplement with session exploration as needed
- If documentation is missing or outdated, note it for later update

### 1. Make Plans from Prompts
- Before implementing any changes, analyze the user's request thoroughly
- Break down complex tasks into clear, actionable steps
- Identify affected files, components, and potential dependencies
- Consider edge cases and potential impacts on existing functionality
- Present a structured plan outlining the proposed approach

### 2. Wait for Approval Before Implementing
- Do not begin coding until the plan has been reviewed and approved
- Allow the user to provide feedback, ask questions, or request modifications
- Clarify any ambiguities before proceeding
- Confirm the scope of changes to avoid unnecessary work

### 3. Update Documentation After Implementation
- After completing the implementation, update relevant documentation
- Document new features, APIs, or configuration options
- Update README files if the changes affect setup or usage
- Add inline comments for complex logic where appropriate
- Ensure any breaking changes are clearly noted
