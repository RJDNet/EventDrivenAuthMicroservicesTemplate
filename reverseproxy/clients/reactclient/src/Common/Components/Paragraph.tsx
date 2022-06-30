import styled from 'styled-components';

interface IParagraphProps {
    red?: string;
    bold?: string;
}

export const Paragraph = styled.p<IParagraphProps>((props: IParagraphProps) => ({
    color: props.red, 
    fontSize: '16px',
    fontWeight: props.bold
}));
